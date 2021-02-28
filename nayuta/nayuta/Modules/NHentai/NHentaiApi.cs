using nayuta.Internal;

namespace nayuta.Modules.NHentai
{
    public static class NHentaiApi
    {
        private static string _apiBaseUrl = "https://nhentai.net/api/";

        public static Book GetBook(string ID)
        {
            if (!int.TryParse(ID, out _))
                return null;

            Book book = APIHelper<Book>.GetData(_apiBaseUrl + "gallery/" + ID);
            book.Build();
            return book;
        }
    }
}