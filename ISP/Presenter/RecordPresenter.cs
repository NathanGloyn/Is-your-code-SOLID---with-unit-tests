using BoxInformation.Interfaces;

namespace BoxInformation.Presenter
{
    public class RecordPresenter
    {
        private readonly IBoxEntry box;

        public RecordPresenter(IBoxEntry boxEntry)
        {
            box = boxEntry;
        }

        public void GetRecordById(string RecordID)
        {
            box.Get(RecordID);
        }

        public void DeleteRecord()
        {
            box.Delete();
        }

        public void UpdateRecord()
        {
            box.Update();
        }

        public void AddRecord()
        {
            box.Add();
        }

        public void DeleteManifest()
        {
            box.DeleteManifest();
        }

        public void DeleteAgreement()
        {
            box.DeleteAgreement();
        }
    }
}
