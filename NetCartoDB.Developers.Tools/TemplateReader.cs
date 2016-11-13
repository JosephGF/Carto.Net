using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NetCarto.Developers.Tools
{
    public interface ITagBlock: ITag
    {
        string TagPatternEnd { get; set; }
        int? RowIndexIni { get; set; }
        int? RowIndexEnd { get; set; }
        string TranslateEnd(string line, string[] data);
        IList<ITag> Childs { get; set; }
    }

    public interface ITagRow : ITag
    {
       int CurrentRowIndex { get; set; }
    }

    public interface ITag
    {
        string TagPattern { get; set; }

        int? ColIndexIni { get; set; }

        string Translate(string line, string[] data);
    }

    internal class Each : ITagBlock
    {

        public string TagPattern { get; set; } = @"\[:EACHROW\((\d),?(\d)?\)\]";
        public string TagPatternEnd { get; set; } = "[:EACHROW/]";

        public int? RowIndexIni { get; set; }
        public int? ColIndexIni { get; set; }
        public int? RowIndexEnd { get; set; }
        public IList<ITag> Childs { get; set; } = new List<ITag>();

        public string Translate(string block, string[] data)
        {
            string result = string.Empty;

            foreach (Match match in TemplateReader.exec(this.TagPattern, block))
            {
                if (match.Success)
                {
                    RowIndexIni = Convert.ToInt32(match.Groups[0]);
                    RowIndexEnd = match.Length == 2 ? Convert.ToInt32(match.Groups[1]) : int.MaxValue;

                    string[] blockLines = block.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    for (int idx = RowIndexIni.Value; idx < data.Length; idx++)
                    {
                        TemplateReader.Reader.Translate(new string[] { data[idx] }, this.Childs);
                    }
                }
            }
            return result;
        }

        public string TranslateEnd(string line, string[] data)
        {
            return string.Empty;
        }
    }

    internal class RowField : ITagRow
    {
        public string TagPattern { get; set; } = @"\[:ROWFIELD\((\d)\)\]";
        public string TagPatternEnd { get; set; } = String.Empty;

        public int? ColIndexIni { get; set; }
        public int CurrentRowIndex { get; set; } = 0;

        public string Translate(string line, string[] data)
        {
            foreach (Match match in TemplateReader.exec(this.TagPattern, line))
            {
                if (match.Success)
                {
                    ColIndexIni = Convert.ToInt32(match.Groups[0]);
                    line = Regex.Replace(line, TagPattern, data[this.CurrentRowIndex].Split(new char[] { ';' })[this.ColIndexIni.Value]);
                }
            }

            return line;
        }
    }

    internal class Field : ITag
    {
        public string TagPattern { get; set; } = @"\[:FIELD\((\d),?(\d)?\)\]";
        public string TagPatternEnd { get; set; } = String.Empty;

        public int? RowIndexIni { get; set; }
        public int? ColIndexIni { get; set; }
        public int? RowIndexEnd { get; set; }
        
        public string Translate(string line, string[] data)
        {
            foreach (Match match in TemplateReader.exec(this.TagPattern, line))
            {
                if (match.Success) {
                    ColIndexIni = Convert.ToInt32(match.Groups[1]);
                    RowIndexIni = Convert.ToInt32(match.Groups[0]);

                    line = Regex.Replace(line, TagPattern, data[RowIndexIni.Value].Split(new char[] { ';' })[ColIndexIni.Value]);
                }
            }

            return line;
        }

        public string TranslateEnd(string line, string[] data)
        {
            return string.Empty;
        }
    }

    internal class TemplateReader
    {
        private StreamReader File { get; set; }
        internal static TemplateReader Reader { get; set; }

        internal static MatchCollection exec(string pattern, string input)
        {
            Regex regexp = new Regex(pattern);
            return regexp.Matches(input);
        } 

        StringBuilder sb = new StringBuilder();
        List<ITag> _tags = new List<ITag>()
        {
            new Each(), new Field()
        };

        string _template = string.Empty;

        private TemplateReader(string file)
        {
            _template = file;
        }

        public static TemplateReader Create(string template)
        {
            Reader = new TemplateReader(template);
            return Reader;
        }

        public string Translate(string csvText, char separator = ';')
        {
            string file = null;
            using (var tmp = new StreamReader(_template))
            {
                file = Translate(tmp, csvText.Split(new char[] { separator }));
            }

            return file;
        }

        public string Translate(StreamReader file, string[] data)
        {
            this.File = file;

            return Translate(data, _tags);
        }

        internal string Translate(string[] data, IList<ITag> tags)
        {
            StringBuilder strFile = new StringBuilder();
            while (!File.EndOfStream)
            {
                string line = File.ReadLine();

                foreach (ITag tag in tags)
                {
                    line = TranslateTag(tag, line, data);
                }

                strFile.AppendLine(line);
            }

            return strFile.ToString();
        }

        internal string TranslateTag(ITag tag, string line, string[] data)
        {
            bool isBlock = tag is ITagBlock;

            if (Regex.IsMatch(line, tag.TagPattern))
            {
                if (isBlock)
                    line = TranslateBlock(tag as ITagBlock, line, data);
                else
                    line = tag.Translate(line, data);
            }

            return line;
        }

        internal string TranslateBlock(ITagBlock tag, string line, string[] data)
        {
            StringBuilder block = new StringBuilder();

            while (!File.EndOfStream)
            {
                string blockLine = File.ReadLine();
                if (Regex.IsMatch(blockLine, tag.TagPatternEnd)) break;
            }

            block.AppendLine(tag.Translate(line, data));
            block.AppendLine(tag.TranslateEnd(line, data));

            return block.ToString();
        }
    }
}
