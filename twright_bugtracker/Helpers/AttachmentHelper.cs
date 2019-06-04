using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace twright_bugtracker.Helpers
{
    public class AttachmentHelper
    {
        public static bool IsWebFriendlyAttachment(HttpPostedFileBase file)
        {
            if (file == null)
                return false;
            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024)
                return false;

            try
            {
                var fileExt = Path.GetExtension(file.FileName);
                return fileExt.Contains(".bmp") ||
                       fileExt.Contains(".png") ||
                       fileExt.Contains(".jpeg") ||
                       fileExt.Contains(".jpg") ||
                       fileExt.Contains(".tiff") ||
                       fileExt.Contains(".pdf") ||
                       fileExt.Contains(".doc") ||
                       fileExt.Contains(".docx") ||
                       fileExt.Contains(".txt") ||
                       fileExt.Contains(".xls") ||
                       fileExt.Contains(".xlsx");

            }
            catch
            {
                return false;
            }

        }

        public static string DisplayImage(string filePath)
        {
            var fileName = filePath;
            switch (Path.GetExtension(filePath))
            {
                case ".doc":
                    fileName = "/Images/defaultdoc.png";
                    break;
                case ".docx":
                    fileName = "/Images/defaultdocx.png";
                    break;
                case ".pdf":
                    fileName = "/Images/defaultpdf.png";
                    break;
                case ".xlsx":
                    fileName = "/Images/defaultxlsx.png";
                    break;
                case ".xls":
                    fileName = "/Images/defaultxls.png";
                    break;
                case ".txt":
                    fileName = "/Images/defaulttxt.png";
                    break;
                default:
                    break;
            }

            return fileName;

        }



    }
}