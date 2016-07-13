namespace MockProjetct.domain
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }

        public User(string nome) {
            this.id = 0;
            this.name = nome;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            User user = (User) obj;

            if (this.id == user.id && this.name == user.name)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
