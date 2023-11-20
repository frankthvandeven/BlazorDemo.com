using System;
using System.Linq.Expressions;

namespace Kenova.Client.Components
{

    public class ValidateEventArgs<TModel> : ValidateEventArgs where TModel : ModelBase
    {

        public bool IsMember(Expression<Func<TModel, object>> expr)
        {
            string fieldName = BindingHelper.GetFieldName(expr);

            if (fieldName == this.FieldName)
                return true;

            return false;
        }

        public bool IsMember(Expression<Func<object>> expr)
        {
            string fieldName = BindingHelper.GetFieldName(expr);

            if (fieldName == this.FieldName)
                return true;

            return false;
        }

    }

}
