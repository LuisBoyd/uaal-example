package com.unity.mynativeapp;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;

import android.util.Patterns;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.URL;
import java.net.URLConnection;
import java.nio.charset.StandardCharsets;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import javax.net.ssl.HttpsURLConnection;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;


public class MainActivity extends AppCompatActivity {

    boolean isUnityLoaded = false;
    EditText m_email;
    EditText m_POIIDField;
    boolean FailedToVerify = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_main);
        Toolbar toolbar = findViewById(R.id.toolbar);

        m_email = findViewById(R.id.editTextTextEmailAddress);
        m_POIIDField = findViewById(R.id.PoiID);
        setSupportActionBar(toolbar);

        handleIntent(getIntent());
    }

    @Override
    protected void onNewIntent(Intent intent) {
        super.onNewIntent(intent);
        handleIntent(intent);
        setIntent(intent);
    }

    void handleIntent(Intent intent) {
        if(intent == null || intent.getExtras() == null) return;

        if(intent.getExtras().containsKey("setColor")){
            View v = findViewById(R.id.button2);
            switch (intent.getExtras().getString("setColor")) {
                case "yellow": v.setBackgroundColor(Color.YELLOW); break;
                case "red": v.setBackgroundColor(Color.RED); break;
                case "blue": v.setBackgroundColor(Color.BLUE); break;
                default: break;
            }
        }
    }

    public void btnLoadUnity(View v) {

        if(!ValidateEmail(m_email))
        {
            return;
        }

        CallBackThread Task = new CallBackThread(
                new ICallbackNoParams() {
                    @Override
                    public void callback() {
                        LoadUnity();
                    }

                    @Override
                    public void FailedCallback() {
                        showToast("Failed To Verify User");
                    }
                }
        , Integer.parseInt(m_POIIDField.getText().toString()),
                m_email.getText().toString(),
                "UK_EnglandWales");


        ExecutorService executor = Executors.newSingleThreadExecutor();

        executor.execute(Task);

    }

    private void LoadUnity(){

        isUnityLoaded = true;
        Intent intent = new Intent(this, MainUnityActivity.class);

        //Creating Bundle this is for testing just dummy locations should be removed
        Bundle bundle = new Bundle();

        bundle.putInt("pointOfInterestId",Integer.parseInt(m_POIIDField.getText().toString())); //Great Haywood Marina Dummy
        bundle.putString("Email", m_email.getText().toString());
        bundle.putString("Region", "UK_EnglandWales");

        intent.putExtras(bundle);

        intent.setFlags(Intent.FLAG_ACTIVITY_REORDER_TO_FRONT);
        startActivityForResult(intent, 1);
    }

    private boolean ValidateEmail(EditText email)
    {
        String emailToText = email.getText().toString();

        if(!emailToText.isEmpty() && Patterns.EMAIL_ADDRESS
                .matcher(emailToText).matches())
        {
            return true;
        }else{
            showToast("Enter Valid Email First");
            return false;
        }
    }

    /*private String Parse(String ResponseBody){
        try {
            JSONArray albums = new JSONArray(ResponseBody);
            for (int i = 0; i < albums.length(); i++){
                JSONObject album = albums.getJSONObject(i);
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }*/

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if(requestCode == 1) isUnityLoaded = false;
    }

    public void unloadUnity(Boolean doShowToast) {
        if(isUnityLoaded) {
            Intent intent = new Intent(this, MainUnityActivity.class);
            intent.setFlags(Intent.FLAG_ACTIVITY_REORDER_TO_FRONT);
            intent.putExtra("doQuit", true);
            startActivity(intent);
            isUnityLoaded = false;
        }
        else if(doShowToast) showToast("Show Unity First");
    }

    public void btnUnloadUnity(View v) {
        unloadUnity(true);
    }

    public void showToast(String message) {
        this.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                CharSequence text = message;
                int duration = Toast.LENGTH_SHORT;
                final Toast toast = Toast.makeText(getApplicationContext(), text, duration);
                toast.show();
            }
        });
    }

    @Override
    public void onBackPressed() {
        finishAffinity();
    }
}
