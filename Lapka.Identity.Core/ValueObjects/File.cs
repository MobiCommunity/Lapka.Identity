﻿using System.IO;

namespace Lapka.Identity.Core.ValueObjects
{
    public class File
    {
        public string Name { get; }
        public Stream Content { get; }
        public string ContentType { get; }

        public File(string name, Stream content, string contentType)
        {
            Name = name;
            Content = content;
            ContentType = contentType;
        }
    }
}