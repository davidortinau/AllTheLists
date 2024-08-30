using System;
using System.Collections.Generic;
using System.Text;

namespace AllTheLists.Models
{
    public class Photo
    {
        public string ImageSrc { get;set;}
        public int Id { get;set; }

        public override string ToString()
        {
            return ImageSrc;
        }
    }
}
