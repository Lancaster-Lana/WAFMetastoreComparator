using System;
using System.Xml.Serialization;

namespace  WAFMetastoreComparator.ENUMS
{
	[Serializable, XmlRoot("FORMMENUACTION", IsNullable = true)]
	public enum FormMenuActionType
	{
		[NonSerialized]
		None = 0,
		ActionAdditionPaidCheckToCheckout,
		ActionRegisterToAdditionCheck,
		ActionCreditCalcToCheckout,
		ActionAdditionCheckToCheckout,
		ActionAdditionSRCheckToCheckout,
		ActionRegisterToAdditionCheckSR,
		ActionCheckoutSaveSSLToSignup,
		ActionCheckoutCreditSaveSSLToSignup,
		ActionOTOSave,
		ActionOTOAffiliateSave,
		ActionOTOPSSave,
		ActionOTOSRSave,
		ActionOTOPaidSave,
		ActionRegisterToAdditionCheckPaid,
		ActionCheckoutSaveSSLToSignupPaid,
		ActionSignupSaveSSLToEmployerPaid,
		ActionPaidSignupSaveSSLToEmployerFree,
		ActionSignupSaveSSLToEmployerSS,
		ActionCheckoutSaveSSLToSignupSR,
		ActionRegisterToAdditionCheckPSP,
		ActionAdditionPSCheckToCheckout,
		ActionCheckoutSaveSSLToSignupPSP,
		ActionRegisterToAdditionCheckAffiliate,
		ActionAdditionCheckAffiliateToCheckout,
		ActionSignupSaveSSLToEmployerAffiliate
	}
}