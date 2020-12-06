using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
namespace OCR
{
  class program
  {
    static void Main()
    {
      var testImagePath = "./1.png";
      StringBuilder Output = new StringBuilder();
      using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.TesseractAndCube ))
      {
        using (var img = Pix.LoadFromFile(testImagePath))
        {
          var i = 1;
          img.ConvertRGBToGray(1,0,0);
          using (var page = engine.Process(img , PageSegMode.Auto))
          {
            var text = page.GetText();
            using (var iter = page.GetIterator()) {
              iter.Begin();
              do {
                if (i % 2 == 0) {
                  if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                  {
                    Output.AppendLine("New block");
                  }
                  if (iter.IsAtBeginningOf(PageIteratorLevel.Para))
                  {
                    Output.AppendLine("New paragraph");
                  }
                  if (iter.IsAtBeginningOf(PageIteratorLevel.TextLine))
                  {
                    Output.AppendLine("New line");
                  }
                  Output.AppendLine("word: " + iter.GetText(PageIteratorLevel.Word));
                }
                i++;
              } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
            }
          }
        }
      }
      Console.Write(Output.ToString());
    }
  }
}
