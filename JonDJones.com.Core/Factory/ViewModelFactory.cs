﻿using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;
using JonDJones.Com.Core.Pages;
using JonDJones.Com.Core.ViewCore;
using JonDJones.com.Core.ViewModel.Shared;
using JonDJones.com.Core.Interfaces;
using EPiServer.Shell.Navigation;
using JonDJones.com.Core.Helpers;
using JonDJones.com.Core.Entities;
using JonDJones.com.Core.Extensions;
using JonDJones.Com.Core.ViewCore.Shared;
using JonDJones.com.Core.Repository;

namespace JonDJones.Com.Core
{
    public class ViewModelFactory
    {
        IBlockHelper _blockHelper;

        IEpiServerDependencies _epiServerDependencies;

        public ViewModelFactory(IBlockHelper blockHelper, IEpiServerDependencies epiServerDependencies)
        {
            _blockHelper = blockHelper;
            _epiServerDependencies = epiServerDependencies;
        }

        public LayoutViewModel CreateLayoutViewModel(PageData currentPage)
        {
            var homePage = _epiServerDependencies.ContentRepository.Get<StartPage>(ContentReference.StartPage);
            
            var layoutModel = new LayoutViewModel
                {
                    HeaderProperties = CreateHeaderProperties(homePage),
                    MetaDataProperties = CreateSeoViewModel(homePage),
                    FooterProperties = CreateFooterProperties(homePage),
                    Menu = CreateMenu(homePage),
                    SiteName = homePage.SiteTitle
                };

            return layoutModel;
        }

        private HeaderViewModel CreateHeaderProperties(IHeaderProperties headrerProperties)
        {
            return headrerProperties == null ? null : new HeaderViewModel(headrerProperties, _epiServerDependencies);
        }

        private IList<NavigationItem> CreateMenu(StartPage startPage)
        {
            var menuRepo = new MenuRepository(_epiServerDependencies);

            return menuRepo.GetMainMenu();
        }

        private NavigationItem CreatNavigationItem(PageData x)
        {
            var navItem = new NavigationItem()
            {
                Name = x.Name,
                Link = x.LinkURL
            };

            return navItem;
        }


        private FooterViewModel CreateFooterProperties(IFooterProperties footerProperties)
        {
            return footerProperties == null ? null : new FooterViewModel(footerProperties, _epiServerDependencies);
        }

        private MetaDataViewModel CreateSeoViewModel(IPageMetaDataProperties metaDataProperties)
        {
            return metaDataProperties == null ? null : new MetaDataViewModel(metaDataProperties, _epiServerDependencies.LinkResolver);
        }
    }

}
