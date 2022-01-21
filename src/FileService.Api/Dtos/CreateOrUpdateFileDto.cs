namespace FileService.Api.Dtos
{
    public sealed class CreateOrUpdateFileDto
    {
        public string Name { get; set; }
        public string ContenteBase64 { get; set; }
    }
}
