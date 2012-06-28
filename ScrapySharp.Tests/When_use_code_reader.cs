// ReSharper disable InconsistentNaming

using System.Diagnostics;
using NUnit.Framework;
using ScrapySharp.Html;

namespace ScrapySharp.Tests
{
    [TestFixture]
    public class When_use_code_reader
    {
        [Test]
        public void When_read_a_simple_tag_with_missing_quote_in_attibute()
        {
            var sourceCode = "<div class=\"login id=\"lol\">test</div>";
            var codeReader = new CodeReader(sourceCode);

            var word = codeReader.ReadWord();
            Assert.AreEqual("<", word.Value);

            word = codeReader.ReadWord();
            Assert.AreEqual("div", word.Value);
            
            word = codeReader.ReadWord();
            Assert.IsTrue(word.IsWhiteSpace);

            word = codeReader.ReadWord();
            Assert.AreEqual("class", word.Value);

            word = codeReader.ReadWord();
            Assert.AreEqual("=", word.Value);

            word = codeReader.ReadWord();
            Assert.AreEqual("login id=", word.Value);

            word = codeReader.ReadWord();
            Assert.AreEqual("lol", word.Value);

            word = codeReader.ReadWord();
            Assert.AreEqual(">", word.Value);

        }

        [Test]
        public void When_read_a_simple_tag()
        {
            var sourceCode = "<div class=\"login box1\">login: \n\t romcy</div>";
            var codeReader = new CodeReader(sourceCode);

            var word = codeReader.ReadWord();
            Assert.AreEqual("<", word.Value);

            word = codeReader.ReadWord();
            Assert.AreEqual("div", word.Value);

            word = codeReader.ReadWord();
            Assert.IsTrue(word.IsWhiteSpace);

            word = codeReader.ReadWord();
            Assert.AreEqual("class", word.Value);
            Assert.IsFalse(word.IsQuoted);

            word = codeReader.ReadWord();
            Assert.AreEqual("=", word.Value);
            
            word = codeReader.ReadWord();
            Assert.AreEqual("login box1", word.Value);
            Assert.IsTrue(word.IsQuoted);

            word = codeReader.ReadWord();
            Assert.AreEqual(">", word.Value);

            word = codeReader.ReadWord();
            Assert.AreEqual("login", word.Value);

            word = codeReader.ReadWord();
            Assert.AreEqual(":", word.Value);

            word = codeReader.ReadWord();
            Assert.AreEqual(" ", word.Value);

            word = codeReader.ReadWord();
            Assert.AreEqual("\n", word.Value);

            word = codeReader.ReadWord();
            Assert.AreEqual("\t", word.Value);

            word = codeReader.ReadWord();
            Assert.AreEqual(" ", word.Value);

            word = codeReader.ReadWord();
            Assert.AreEqual("romcy", word.Value);


            word = codeReader.ReadWord();
            Assert.AreEqual("</", word.Value);

            word = codeReader.ReadWord();
            Assert.AreEqual("div", word.Value);

            word = codeReader.ReadWord();
            Assert.AreEqual(">", word.Value);

        }
    }
}

// ReSharper restore InconsistentNaming
