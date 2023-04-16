
public class Mariana
{
    
    public int Id { get; set; } //Also add details about what the marina is but could just pull that from other DB
    
    public int pointOfIntrestID { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public string telnumber { get; set; }
    public string website { get; set; }
    public string address { get; set; }
    public string facilities { get; set; }
    public string email { get; set; }
    public string p_canal { get; set; }
    public string s_canal { get; set; }
    public string W3W { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
    public double cos_lat { get; set; }
    public double cos_lng { get; set; }
    public double sin_lat { get; set; }
    public double sin_lng { get; set; }
    public int linkedCanalPointid { get; set; }
}