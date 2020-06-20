using DeepMorphy;
using System.Collections.Generic;
using System.Linq;

namespace PePets.Services
{
    public class MorphyService
    {
        private MorphAnalyzer morph;

        private Dictionary<string, int> profile = new Dictionary<string, int>
        {
            // Наиболее значимые части речи
            { "сущ", 5 },
            { "гл", 5 },
            { "инф_гл", 5 },
            { "прил", 3 },
            { "кр_прил", 3 },
            { "комп", 3 },
            { "прич", 3 },
            { "кр_прич", 3 },
            { "деепр", 3 },
            { "нареч", 3 },

            // Служебные части речи 
            { "предл", 0 },
            { "союз", 0 },
            { "межд", 0 },
            { "числ", 0 },
            { "част", 0 },
            { "мест", 0 },
            { "предик", 0 },
            { "пункт", 0 },
            { "цифра", 0 },
            { "рим_цифр", 0 },

            // Неизвестные части речи 
            { "неизв", 1 }
        };

        public MorphyService()
        {
            morph = new MorphAnalyzer(withLemmatization: true);
        }

        // Преобразование текста к нормализованному виду для индексации и поиска
        public string GetGeneralText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            // Получение массива базовых слов
            string[] textLemmas = Lemmatize(text);

            // Очистка массива слов от ненужных слов
            string[] cleanWords = ClearWords(textLemmas);

            // Формирование строки из базовых слов
            return string.Join(' ', cleanWords);
        }

        // Получение массива слов из текста
        public string[] GetWordsFromText(string text)
        {
            // Удаление пробелов в начале и конце строки и перевод в нижний регистр
            text = text.Trim().ToLower();

            // Замена букв ё на е
            text = text.Replace('ё', 'е');

            // Разделене слов по пробелу
            return text.Split(' ');
        }

        // Поиск базовой формы для каждого слова
        public string[] Lemmatize(string text)
        {
            string[] words = GetWordsFromText(text);

            List<string> lemmaWords = new List<string>();
            foreach (string word in words)
            {
                // Получение информации о слове 
                var results = morph.Parse(new string[] { word }).ToArray();

                // Добавление базовой формы в список
                lemmaWords.Add( results[0].BestTag.Lemma );
            }

            return lemmaWords.ToArray();
        }

        // Очистка массива слов от ненужных
        private string[] ClearWords(string[] words)
        {
            List<string> newWords = new List<string>();
            foreach (string word in words)
            {
                if (Validate(word))
                    newWords.Add(word);
            }

            return newWords.ToArray();
        }

        // Валидация слова
        private bool Validate(string word)
        {
            if (word == "купить" || word == "продать")
                return false;

            var result = morph.Parse(new string[] { word }).ToArray();
            string partOfSpeech = result[0].BestTag["чр"];
            if (profile[partOfSpeech] == 0)
                return false;

            return true;
        }
    }
}
