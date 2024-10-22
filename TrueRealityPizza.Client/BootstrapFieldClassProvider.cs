﻿using Microsoft.AspNetCore.Components.Forms;

namespace TrueRealityPizza.Client;

public class BootstrapFieldClassProvider : FieldCssClassProvider
{
	public override string GetFieldCssClass(EditContext editContext,
			in FieldIdentifier fieldIdentifier)
	{
		var isValid = editContext.IsValid(fieldIdentifier);

		return isValid ? "is-valid" : "is-invalid";
	}
}