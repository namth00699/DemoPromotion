using EPiServer.Shell.ObjectEditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAlloySite.Config
{
    public class SelectOneEnumAttribute : SelectOneAttribute, IMetadataAware
    {
        public SelectOneEnumAttribute(Type enumType)
        {
            EnumType = enumType;
        }

        public Type EnumType { get; set; }

        public new void OnMetadataCreated(ModelMetadata metadata)
        {
            var enumType = metadata.ModelType;

            SelectionFactoryType = typeof(EnumSelectionFactory<>).MakeGenericType(EnumType);

            base.OnMetadataCreated(metadata);
        }
    }
}
