﻿using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using Structure.Sketching;
using Structure.Sketching.Filters.Resampling;
using Structure.Sketching.Filters.Resampling.Enums;
using File = Coolector.Services.Remarks.Domain.File;

namespace Coolector.Services.Remarks.Services
{
    public interface IImageService
    {
        IDictionary<string,File> ProcessImage(File file);
    }


    public class ImageService : IImageService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const double SmallSize = 640;
        private const double MediumSize = 1024;
        private const double BigSize = 1600;

        public IDictionary<string, File> ProcessImage(File file)
        {
            Logger.Debug($"Processing image, name:{file.Name}, contentType:{file.ContentType}, " +
                         $"sizeBytes:{file.SizeBytes}");

            using (var stream = new MemoryStream(file.Bytes))
            {
                var originalImage = new Image(stream);
                var smallImage = ScaleImage(originalImage, SmallSize);
                var mediumImage = ScaleImage(originalImage, MediumSize);
                var bigImage = ScaleImage(originalImage, BigSize);

                var dictionary = new Dictionary<string, File>
                {
                    {"small", File.Create(file.Name, file.ContentType, smallImage.Pixels)},
                    {"medium", File.Create(file.Name, file.ContentType, mediumImage.Pixels)},
                    {"big", File.Create(file.Name, file.ContentType, bigImage.Pixels)},
                    {"original", file}
                };

                return dictionary;
            }
        }

        private Image ScaleImage(Image image, double maxSize)
        {
            var ratioX = maxSize/image.Width;
            var ratioY = maxSize/image.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int) (image.Width*ratio);
            var newHeight = (int) (image.Height*ratio);

            return new Image(image).Apply(new Resize(newWidth, newHeight, ResamplingFiltersAvailable.Cosine));
        }
    }
}