using System.Data;

namespace BoxInformation.Interfaces
{
    public interface IBoxEntry
    {
        void Get(string boxId);
        void Delete();
        void Update();
        void Add();
        void DeleteManifest();
        void DeleteAgreement();
        void PopulateView(DataRow entry);
    }
}