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
            // Служебные части речи //
            { "ПРЕДЛ", 0 },
            { "СОЮЗ", 0 },
            { "МЕЖД", 0 },
            { "ВВОДН", 0 },
            { "ЧАСТ", 0 },
            { "МС", 0 },

            // Наиболее значимые части речи //
            { "сущ", 5 },
            { "инф_гл", 5 },
            { "прил", 3 },
            { "нар", 3 },

            // Остальные части речи //
            { "DEFAULT", 1 }
        };

        public MorphyService()
        {
            morph = new MorphAnalyzer(withLemmatization: true);
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
    }
}
