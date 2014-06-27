﻿using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Activities.RetailPriceDiscount.Controllers
{
    public class ConfigController : CommerceControllerBase
    {
        private IRepository<ActivityRule> _rules;

        public ConfigController(IRepository<ActivityRule> rules)
        {
            _rules = rules;
        }

        public ActionResult Load(int ruleId, int attachedActivityInfoId)
        {
            var rule = _rules.Find(ruleId);
            var attachedActivityInfo = rule.AttachedActivityInfos.Find(attachedActivityInfoId);
            var config = attachedActivityInfo.LoadActivityConfig<RetailPriceDiscountActivityConfig>();
            return JsonNet(config).UsingClientConvention();
        }

        [HttpPost, Transactional]
        public void Save(int ruleId, int attachedActivityInfoId, RetailPriceDiscountActivityConfig config)
        {
            var rule = _rules.Find(ruleId);
            var attachedActivityInfo = rule.AttachedActivityInfos.Find(attachedActivityInfoId);
            attachedActivityInfo.UpdateActivityConfig(config);
        }
    }
}
