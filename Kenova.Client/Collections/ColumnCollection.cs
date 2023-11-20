using Kenova.Client.Util;
using System;
using System.Linq.Expressions;

namespace Kenova.Client.Components
{
    public class ColumnCollection<ItemType> : MonitoredCollection<Column<ItemType>>
    {

        public int Add(Expression<Func<ItemType, object>> field, string header, double width = 150.0d, bool display_as_hyperlink = false, bool sortable = true)
        {
            // FieldExpression="c => c.ddDatumRecept" Header="ddDatumRecept" Width="150" DisplayAsHyperlink

            var col = new Column<ItemType>
            {
                Kind = ColumnKind.Field,
                FieldExpression = field,
                Header = header,
                Width = width,
                DisplayAsHyperlink = display_as_hyperlink,
                Sortable = sortable
            };

            this.Add(col);

            return (this.Count - 1);
        }

        public int AddIcon(Expression<Func<ItemType, IconDefinition>> icon_definition)
        {
            var col = new Column<ItemType>
            {
                Kind = ColumnKind.IconDefinition,
                IconDefinitionExpression = icon_definition,
                Width = 24,
                Sortable = false
            };

            this.Add(col);

            return (this.Count - 1);
        }

        //public string IconData
        //{
        //    get
        //    {
        //        if (_current == null)
        //            throw new NullReferenceException();

        //        return _current.IconData;
        //    }
        //    set
        //    {
        //        if (_current == null)
        //            throw new NullReferenceException();

        //        _current.IconData = value;
        //    }
        //}


    }

    public class Column<ItemType>
    {
        public ColumnKind Kind { get; set; }

        private Expression<Func<ItemType, object>> _fieldExpression;
        private Func<ItemType, object> _fieldExpressionCompiled;
        private string _fieldname;

        private Expression<Func<ItemType, IconDefinition>> _iconDefinitionExpression;
        private Func<ItemType, IconDefinition> _iconDefinitionExpressionCompiled;

        public Expression<Func<ItemType, object>> FieldExpression
        {
            get { return _fieldExpression; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("FieldExpression");

                _fieldExpression = value;
                _fieldExpressionCompiled = value.Compile();
                _fieldname = LambdaHelper.GetMemberName(_fieldExpression.Body);

                if (Header == null)
                    Header = _fieldname;
            }

        }

        public Expression<Func<ItemType, IconDefinition>> IconDefinitionExpression
        {
            get { return _iconDefinitionExpression; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("IconDefinitionExpression");

                _iconDefinitionExpression = value;
                _iconDefinitionExpressionCompiled = value.Compile();
            }
        }

        public string Header { get; set; }

        public bool DisplayAsHyperlink { get; set; } = false;

        public bool Sortable { get; set; } = true;

        /// <summary>
        /// Minimum width in pixels.
        /// </summary>
        public double Width { get; set; } = 80;

        public string FieldName
        {
            get { return _fieldname; }
        }


        public object Value(ItemType item)
        {
            if (this.Kind != ColumnKind.Field)
                return null;

            return _fieldExpressionCompiled(item);
        }

        public string ValueToHtml(ItemType item)
        {
            if (this.Kind != ColumnKind.Field)
                return "";

            object value = _fieldExpressionCompiled(item);

            if (value == null)
                return "";

            if (value is DateTime)
            {
                return ((DateTime)value).ToShortDateString();
            }

            return value.ToString();
        }

        //public string XXXValueToHTML(object value_as_object)
        //{
        //    byte[] image = value_as_object as byte[];

        //    if (image == null)
        //        throw new ArgumentException("value_as_object is not of type byte[]");

        //    string base64_string = System.Convert.ToBase64String(image);

        //    return $"<img src=\"data:image;base64,{base64_string}\" width=\"100%\" />";
        //}


        public IconDefinition GetIconDefinition(ItemType item)
        {
            return _iconDefinitionExpressionCompiled(item);
        }

    }

    public enum ColumnKind
    {
        Field,
        IconDefinition
    }

}
