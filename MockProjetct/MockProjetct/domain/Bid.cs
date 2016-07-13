namespace MockProjetct.domain
{
    public class Bid
    {
        public User user { get; set; }
        public double value { get; set; }
        public int id { get; set; }

        public Bid(User user, double value)
        {
            this.user = user;
            this.value = value;
        }
    }
}
