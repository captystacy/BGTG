using POS.Infrastructure.Services.DocumentServices.WordService.Format;
using Spire.Doc;

namespace POS.Infrastructure.Services.DocumentServices.WordService.Base
{
    public abstract class MyBody : IMyBody
    {
        public MyCharacterFormat? CharacterFormat { get; set; }
        public MyParagraphFormat? ParagraphFormat { get; set; }

        protected abstract Body Body { get; }

        public IReadOnlyList<IMyParagraph> Paragraphs { get; private set; }

        protected MyBody(IReadOnlyList<IMyParagraph> paragraphs)
        {
            Paragraphs = paragraphs;
        }

        protected MyBody(IReadOnlyList<IMyParagraph> paragraphs, MyParagraphFormat? paragraphFormat, MyCharacterFormat? characterFormat)
        {
            ParagraphFormat = paragraphFormat;
            CharacterFormat = characterFormat;
            Paragraphs = paragraphs;
        }

        public IMyParagraph AddParagraph()
        {
            return AddParagraph(null!);
        }

        public IMyParagraph AddParagraph(string? text)
        {
            return AddParagraph(text, ParagraphFormat, CharacterFormat);
        }

        public IMyParagraph AddParagraph(string? text, MyParagraphFormat? paragraphFormat)
        {
            return AddParagraph(text, paragraphFormat, CharacterFormat);
        }

        public IMyParagraph AddParagraph(string? text, MyCharacterFormat? characterFormat)
        {
            return AddParagraph(text, ParagraphFormat, characterFormat);
        }

        public IMyParagraph AddParagraph(string? text, MyParagraphFormat? paragraphFormat, MyCharacterFormat? characterFormat)
        {
            var paragraph = Body.AddParagraph();

            var myParagraph = new MyParagraph(paragraph);

            if (paragraphFormat is not null)
            {
                myParagraph.ApplyFormat(paragraphFormat);
            }

            if (text is not null)
            {
                var myTextRange = myParagraph.AppendText(text);

                if (characterFormat is not null)
                {
                    myTextRange.ApplyFormat(characterFormat);
                }
            }

            return AddParagraphToMyList(myParagraph);
        }

        private IMyParagraph AddParagraphToMyList(IMyParagraph myParagraph)
        {
            var paragraphs = Paragraphs.ToList();
            paragraphs.Add(myParagraph);

            Paragraphs = paragraphs;

            return myParagraph;
        }
    }
}