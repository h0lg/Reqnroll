using System.Globalization;
using Reqnroll.Assist;
using Reqnroll.Bindings;
using Reqnroll.Bindings.Reflection;
using Reqnroll.BoDi;

namespace StepArgumentTransformationInTableCells.Support
{
    /// <summary>
    /// Custom value retriever applying <see cref="StepArgumentTransformationAttribute"/> to <see cref="Table"/> cells
    /// to be used with <see cref="TableHelperExtensionMethods.CreateSet{T}(Table)"/>
    /// or <see cref="TableHelperExtensionMethods.CreateInstance{T}(Table, Func{T})"/> or their overloads.
    /// Originally inspired by https://gist.githubusercontent.com/gasparnagy/a478e5b7ccb8f557a6dc/ .
    /// </summary>
    internal sealed class TableCellStepArgumentConverterValueRetriever : IValueRetriever
    {
        /// <summary>Sets up the step argument conversion infrastructure.</summary>
        /// <remarks>This method has to be called once, in a test ctor for example.
        /// Note this way of setting is not supported for parallel execution.</remarks>
        internal static void Register(IObjectContainer container)
        {
            // prevent re-registration
            if (Service.Instance.ValueRetrievers.Any(vr => vr is TableCellStepArgumentConverterValueRetriever)) return;

            // unregistering all existing retrievers
            foreach (var valueRetriever in Service.Instance.ValueRetrievers.ToArray())
                Service.Instance.ValueRetrievers.Unregister(valueRetriever);

            // register retriever that uses the step argument conversion mechanism
            Service.Instance.ValueRetrievers.Register(new TableCellStepArgumentConverterValueRetriever(container));
        }

        private readonly IStepArgumentTypeConverter converter;

        private TableCellStepArgumentConverterValueRetriever(IObjectContainer container)
            => converter = container.Resolve<IStepArgumentTypeConverter>();

        private static RuntimeBindingType GetTargetType(Type propertyType)
            => new(Nullable.GetUnderlyingType(propertyType) ?? propertyType);

        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
            => converter.CanConvert(keyValuePair.Value, GetTargetType(propertyType), CultureInfo.CurrentCulture);

        public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
            => converter.ConvertAsync(keyValuePair.Value, GetTargetType(propertyType), CultureInfo.CurrentCulture).Result;
    }
}
