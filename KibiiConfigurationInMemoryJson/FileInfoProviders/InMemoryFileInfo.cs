#region License Information (MIT)

/*
    MIT License

    Copyright (c) 2018 Kibii Inc.

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.

*/

#endregion

/* Forked from https://github.com/adams85/bundling/blob/master/source/Bundling/Internal/Caching/MemoryFileInfo.cs */
/* Author: adams85. Minor changes made by Kibii Inc. */

using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace Kibii.Extensions.Configuration.InMemoryJson.FileInfoProviders
{
    public class InMemoryFileInfo : IFileInfo
    {
        private readonly byte[] _content;

        public InMemoryFileInfo(string name, string content, DateTimeOffset? timestamp = null) : this(name, Encoding.UTF8.GetBytes(content), timestamp) { }
        public InMemoryFileInfo(string name, byte[] content, DateTimeOffset? timestamp = null)
        {
            Name = name;
            _content = content;
            LastModified = timestamp ?? DateTimeOffset.Now;
        }

        public bool Exists => true;
        public long Length => _content.LongLength;
        public string PhysicalPath => null;
        public string Name { get; }
        public DateTimeOffset LastModified { get; }
        public bool IsDirectory => false;

        public Stream CreateReadStream()
        {
            return new MemoryStream(_content);
        }
    }
}
