namespace mf_api_web_services_fuel_manager.Models
{
    public class LinkDto
    {
        public int Id { get; set; }
        public string Href { get; set; }
        public string Real { get; set; }
        public string Metodo { get; set; }

        public LinkDto(int id, string href, string real, string metodo)
        {
            Id = id;
            Href = href;
            Real = real;
            Metodo = metodo;
        }

    }

    public class  LinkHATEOS 
    {
        public List <LinkDto>Links { get; set; = new List <LinkDto>();> }
        
    }
}
