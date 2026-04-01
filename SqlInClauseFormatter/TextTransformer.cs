using System;
using System.Linq;

namespace SqlInClauseFormatter
{
    /// <summary>
    /// 줄바꿈으로 구분된 텍스트를 SQL IN절 형식으로 변환하는 순수 변환 클래스
    /// </summary>
    public static class TextTransformer
    {
        /// <summary>
        /// 줄바꿈 구분 텍스트를 SQL IN절로 변환
        ///
        /// 입력:
        /// A
        /// B
        /// C
        ///
        /// 출력:
        /// (
        /// 'A',
        /// 'B',
        /// 'C'
        /// )
        /// </summary>
        public static string ConvertToInClause(string selectedText)
        {
            if (string.IsNullOrWhiteSpace(selectedText))
                return selectedText;

            var lines = selectedText
                .Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None)
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line))
                .ToArray();

            if (lines.Length == 0)
                return selectedText;

            // 각 값에 작은따옴표 씌우기 (값 안의 ' 는 '' 로 이스케이프)
            var quoted = lines.Select(val => "'" + val.Replace("'", "''") + "'");

            return "(\r\n" + string.Join(",\r\n", quoted) + "\r\n)";
        }
    }
}
