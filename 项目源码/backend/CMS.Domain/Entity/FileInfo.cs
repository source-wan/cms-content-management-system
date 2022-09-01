namespace CMS.Domain.Entity
{
    public class FileInfo:Base.BaseEntity
    {
        public string OriginName {get;set;}=null!;
        
        public string CurrentName {get;set;}=null!;

        public string RelativePath {get;set;}=null!;

        public string ContentType {get;set;}=null!;

        public string FileSize {get;set;}=null!;
    }
}