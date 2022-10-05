﻿using EPiServer;
using EPiServer.Core;
using MyAlloySite.Facets;
using MyAlloySite.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyAlloySite.Constant.Constants;

namespace MyAlloySite.Config
{
    public interface IFacetConfigFactory
    {
        List<FacetDefinition> GetDefaultFacetDefinitions();

        List<FacetFilterConfigurationItem> GetFacetFilterConfigurationItems();

        FacetDefinition GetFacetDefinition(FacetFilterConfigurationItem facetConfiguration);
    }

    public class FacetConfigFactory : IFacetConfigFactory
    {
        private readonly IContentLoader _contentLoader;

        public FacetConfigFactory(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public virtual List<FacetDefinition> GetDefaultFacetDefinitions()
        {
            return new List<FacetDefinition>();
        }

        public virtual FacetDefinition GetFacetDefinition(FacetFilterConfigurationItem facetConfiguration)
        {
            switch (Enum.Parse(typeof(FacetFieldType), facetConfiguration.FieldType))
            {
                case FacetFieldType.String:
                    return new FacetStringDefinition
                    {
                        FieldName = facetConfiguration.FieldName,
                        DisplayName = facetConfiguration.GetDisplayName()
                    };

                case FacetFieldType.ListOfString:
                    return new FacetStringListDefinition
                    {
                        FieldName = facetConfiguration.FieldName,
                        DisplayName = facetConfiguration.GetDisplayName()
                    };

                case FacetFieldType.Boolean:
                case FacetFieldType.NullableBoolean:
                    return new FacetStringListDefinition
                    {
                        FieldName = facetConfiguration.FieldName,
                        DisplayName = facetConfiguration.GetDisplayName(),
                    };
            }

            return new FacetStringDefinition
            {
                FieldName = facetConfiguration.FieldName,
                DisplayName = facetConfiguration.GetDisplayName(),
            };
        }

        public List<FacetFilterConfigurationItem> GetFacetFilterConfigurationItems()
        {
            if (ContentReference.IsNullOrEmpty(ContentReference.StartPage))
            {
                return new List<FacetFilterConfigurationItem>();
            }

            var startPage = _contentLoader.Get<IContent>(ContentReference.StartPage);

            var facetsConfiguration = startPage as IFacetConfiguration;
            if (facetsConfiguration?.SearchFiltersConfiguration != null)
            {
                return facetsConfiguration
                    .SearchFiltersConfiguration
                    .ToList();
            }

            return new List<FacetFilterConfigurationItem>();
        }
    }
}
