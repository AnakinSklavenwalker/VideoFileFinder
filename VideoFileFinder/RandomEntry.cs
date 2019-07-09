﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Microsoft.WindowsAPICodePack.Shell;

namespace VideoFileFinder
{
    public struct RandomEntry : IEquatable<RandomEntry>
    {
        public IEnumerable<FileEntry> Files { get; }

        public string Filter { get; }

        public string Drives { get; }

        public bool LogicalOr { get; }

        public RandomEntry(IEnumerable<string> files, string filter, string drives, bool logicalOr)
        {
            Filter = filter;
            Drives = drives;
            LogicalOr = logicalOr;
            Files = files.Select(x => new FileEntry(x)).ToList();
        }

        public bool Equals(RandomEntry other)
        {
            return string.Equals(Filter, other.Filter) && string.Equals(Drives, other.Drives) && LogicalOr == other.LogicalOr;
        }

        public override bool Equals(object obj)
        {
            return obj is RandomEntry other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Filter != null ? Filter.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Drives != null ? Drives.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ LogicalOr.GetHashCode();
                return hashCode;
            }
        }
    }

    public struct FileEntry
    {
        public string FilePath { get; }

        public ImageSource Thumbnail { get; }

        public string Tags { get; }

        public FileEntry(string path)
        {
            FilePath = path;
            var file = ShellFile.FromFilePath(path);
            Thumbnail = file.Thumbnail.BitmapSource;
            var tags = file.Properties.System.Keywords.Value;
            Tags = tags == null ? string.Empty : string.Join("; ", file.Properties.System.Keywords.Value);
        }
    }
}