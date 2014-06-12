using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Pricing;
using Kooboo.Commerce.Orders.Pricing.Stages;
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
            using (var scope = PricingContext.Begin(context))
            {
                Prepare(context);

                var stages = _stageTypes.Select(type => CreatePricingStage(type)).ToList();

                Event.Raise(new PriceCalculationStarted(stages));

                foreach (var stage in stages)
                {
                    stage.Execute(context);
                    Event.Raise(new PriceCalculationStageCompleted(stage.Name));
                }

                Event.Raise(new PriceCalculationCompleted());
            }
        }

        private IPricingStage CreatePricingStage(Type stageType)
        {
            return (IPricingStage)EngineContext.Current.Resolve(stageType);
        }

        private void Prepare(PricingContext context)
        {
            context.Subtotal.SetOriginalValue(context.Items.Sum(x => x.Subtotal.OriginalValue));
        }

        public static PricingPipeline Create()
        {
            return new PricingPipeline();
        }

        public static readonly PricingStageTypeCollection DefaultStages = new PricingStageTypeCollection(new Type[]
        {
            typeof(PaymentMethodPricingStage),
            typeof(ShippingPricingStage),
            typeof(TaxPricingStage),
            typeof(PromotionPricingStage)
        });
    }
}
