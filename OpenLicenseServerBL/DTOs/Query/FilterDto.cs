namespace OpenLicenseServerBL.DTOs
{
    public class FilterDto
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string SortCriteria { get; set; }
        public bool SortDescending { get; set; }
    }
}
