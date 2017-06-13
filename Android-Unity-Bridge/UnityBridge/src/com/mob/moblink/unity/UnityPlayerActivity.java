package com.mob.moblink.unity;

import android.content.Intent;
import android.os.Bundle;

public class UnityPlayerActivity extends com.unity3d.player.UnityPlayerActivity {

	@Override
	protected void onCreate(Bundle bundle) {
		super.onCreate(bundle);
	}

	@Override
	protected void onNewIntent(Intent intent) {
		super.onNewIntent(intent);
		Intent oldIntent = getIntent();
		if (oldIntent != intent) {
			setIntent(intent);
		}
	}
}
