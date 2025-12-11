namespace Models
{
    public class Utilizador
    {
        public int Id { get; set; }
        public string Auth0UserId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string ImgUrl { get; set; }
    }
}
