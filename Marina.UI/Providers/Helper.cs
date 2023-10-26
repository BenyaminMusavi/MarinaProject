//using Marina.UI.Models.Entities;
//using Marina.UI.Providers.Repositories;
//using Microsoft.AspNetCore.Mvc.Rendering;

//namespace Marina.UI.Providers
//{
//    public static class Helper
//    {
//        //private readonly IRegionRepository _regionRepository;

//        //public Helper(IRegionRepository regionRepository)
//        //{
//        //    _regionRepository = regionRepository;
//        //}

//        public static List<SelectListItem> GetRegionList(List<Region> region) //string selectedItem = null, bool addEmptyItem = false
//        {
//            var items = new List<SelectListItem>();
//            //try
//            //{
//            if (true)
//                items.Add(new SelectListItem { Text = "", Value = "0" });

//            var apiService = _regionRepository.GetAll();
//            //var response = true;
//            if (region is not null) //response
//            {
//                foreach (var item in region)
//                {
//                    var optionItem = new SelectListItem
//                    {
//                        Text = item.Name,
//                        Value = item.Id.ToString(),
//                    };
//                    //optionItem.Selected = selectedItem.HasValue() ? selectedItem == optionItem.Value : false;

//                    items.Add(optionItem);
//                }


//            }
//            //}
//            //catch (Exception ex)
//            //{
//            //    getLogger().Error(ex);
//            //}
//            return items;
//        }
//    }
//}
