using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev._Helper
{
    public class PdfConverter
    {
       
            private string myPath;
            private Guid myGuid;
            private FrameDimension myDimension;
            public ArrayList myImages = new ArrayList();
            private int myPageCount;
            private Bitmap myBMP;

            public PdfConverter(string path)
            {
                MemoryStream ms;
                Image myImage;

                myPath = path;
                FileStream fs = new FileStream(myPath, FileMode.Open);
                myImage = Image.FromStream(fs);
                myGuid = myImage.FrameDimensionsList[0];
                myDimension = new FrameDimension(myGuid);
                myPageCount = myImage.GetFrameCount(myDimension);
                for (int i = 0; i < myPageCount; i++)
                {
                    ms = new MemoryStream();
                    myImage.SelectActiveFrame(myDimension, i);
                    myImage.Save(ms, ImageFormat.Bmp);
                    myBMP = new Bitmap(ms);
                    myImages.Add(myBMP);
                    ms.Close();
                }
                fs.Close();
            }
        
    }
}
