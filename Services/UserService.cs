namespace BookManagementSystem.Services
{
    public class UserService
    {
        //Not use anymore, leave here just for reference for password creating without .net identity

        //ApplicationDbContext _db;

        //public UserService(ApplicationDbContext db)
        //{
        //    _db = db;
        //}

        //public string getHashFromPassword(string sPwd, string sSalt)
        //{
        //    var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(sPwd),
        //        Convert.FromHexString(sSalt), IntegerConst.iIteration, HashAlgorithmName.SHA512, IntegerConst.iKeySize);
        //    var sHash = Convert.ToHexString(hash);
        //    return sHash;
        //}

        //public string getSaltForHash(string sPwd)
        //{
        //    var salt = RandomNumberGenerator.GetBytes(IntegerConst.iKeySize);
        //    string sSalt = Convert.ToHexString(salt);
        //    return sSalt;
        //}


        //public bool verifyPassword(string sPwd, string sHash, string sSalt)
        //{
        //    var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(sPwd, Convert.FromHexString(sSalt),
        //        IntegerConst.iIteration, HashAlgorithmName.SHA512, IntegerConst.iKeySize);
        //    return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(sHash));
        //}
    }
}
