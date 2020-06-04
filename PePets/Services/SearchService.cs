using Nest;
using PePets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Services
{
    public class SearchService
    {
        private readonly ElasticClient _elasticClient;
        private readonly MorphyService _morphyService;

        public SearchService()
        {
            _morphyService = new MorphyService();

            // Подключение к ElasticSearch
            var elasticSetting = new ConnectionSettings(new Uri("http://localhost:9200"))
                .DefaultMappingFor<PostIndex>(m => m.IndexName("posts"))
                .DefaultIndex("posts");
            _elasticClient = new ElasticClient(elasticSetting);
        }

        // Индексирование объявления
        public async Task<IndexResponse> IndexPost(Advert post)
        {
            // Получение массива базовых слов
            string[] titleLemmaWords = _morphyService.Lemmatize(post.Title);
            string[] descriptionLemmaWords = _morphyService.Lemmatize(post.Description);

            PostIndex postIndex = new PostIndex
            {
                Id = post.Id,
                Title = string.Join(' ', titleLemmaWords),
                Description = string.Join(' ', descriptionLemmaWords)
            };

            return await _elasticClient.IndexDocumentAsync(postIndex);
        }
        
        // Поиск по объявлениям (возвращает список id найденных объявлений)
        public async Task<List<Guid>> Search(string searchText)
        {
            // Получение массива базовых слов
            string[] searchLemmaWords = _morphyService.Lemmatize(searchText);

            // Формирование строки поиска из базовых слов
            string search = string.Join(' ', searchLemmaWords);

            // Запрос поиска
            var response = await _elasticClient.SearchAsync<PostIndex>(s => s
                .Index("posts")
                .From(0)
                .Size(10)
                .Query(q => q
                    .Match(mq => mq.Field(f => f.Title).Query(search)) || q
                    .Match(mq => mq.Field(f => f.Description).Query(search))
                )
                .Sort(s => s.Descending(SortSpecialField.Score))
            );

            // Формирование списка идентификаторов найденных объявлений
            List<Guid> ids = new List<Guid>();
            foreach (var hit in response.Hits)
                ids.Add(Guid.Parse(hit.Id));

            return ids;
        }

        public async Task<DeleteResponse> DeleteIndex(string id)
        {
            return await _elasticClient.DeleteAsync(new DeleteRequest<PostIndex>(id));
        }
    }
}
