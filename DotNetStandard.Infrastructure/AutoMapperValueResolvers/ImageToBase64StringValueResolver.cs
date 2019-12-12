using AutoMapper;
using System;

namespace DotNetStandard.Infrastructure.AutoMapperValueResolvers
{
    public class ImageToBase64StringValueResolver : IValueResolver<ImageModel, ImageViewModel, string>
    {
        public string Resolve(ImageModel source, ImageViewModel destination, string destMember, ResolutionContext context)
        {
            return $"data:{source.FileType};base64, {Convert.ToBase64String(source.Image)}";
        }
    }

    public class ImageModel
    {
        public string FileType { get; set; }
        public byte[] Image { get; set; }
    }

    public class ImageViewModel
    {

    }
}
