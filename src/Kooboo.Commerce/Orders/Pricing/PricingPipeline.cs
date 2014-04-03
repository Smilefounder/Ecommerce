using Kooboo.Commerce.Orders.Pricing.Stages;
using Kooboo.Commerce.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing
{
    /// <summary>
    /// Represents a price calculation pipeline in which each stage calculates a portion of the price.
    /// </summary>
    public class PricingPipeline
    {
        private List<Type> _stageTypes;
        private ITypeActivator _typeActivator = new DefaultTypeActivator();

        public ITypeActivator TypeActivator
        {
            get
            {
                return _typeActivator;
            }
            set
            {
                _typeActivator = value;
            }
        }

        public PricingPipeline()
        {
            _stageTypes = new List<Type>(DefaultStages);
        }

        public void Add<TStage>()
            where TStage : IPricingStage
        {
            _stageTypes.Add(typeof(TStage));
        }

        public void Remove<TStage>()
        {
            _stageTypes.Remove(typeof(TStage));
        }

        public void Execute(PricingContext context)
        {
            var stages = _stageTypes.Select(type => (IPricingStage)_typeActivator.Activate(type)).ToList();
            foreach (var stage in stages)
            {
                stage.Execute(context);
            }
        }

        public static PricingPipeline Create()
        {
            return new PricingPipeline();
        }

        public static readonly PricingStageTypeCollection DefaultStages = new PricingStageTypeCollection(new Type[]
        {
            typeof(PreparingPricingStage),
            typeof(PaymentMethodPricingStage),
            typeof(ShippingPricingStage),
            typeof(TaxPricingStage),
            typeof(PromotionPricingStage)
        });
    }
}
