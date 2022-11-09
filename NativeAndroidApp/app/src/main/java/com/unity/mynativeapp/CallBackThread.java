package com.unity.mynativeapp;

import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.URL;
import java.nio.charset.StandardCharsets;

import javax.net.ssl.HttpsURLConnection;

public class CallBackThread implements Runnable{

    ICallbackNoParams c;
    int POIid;
    String UserKey;
    String region;

    public CallBackThread(ICallbackNoParams c, int POIid, String UserKey, String region){
        this.c = c;
        this.POIid = POIid;
        this.UserKey = UserKey;
        this.region = region;
    }

    @Override
    public void run() {
        HttpsURLConnection urlConnection = null;
        boolean error = false;
        try{

            URL url = new URL("https://waternav.co.uk/WaterNavGame/VerifyEntry.php");
            urlConnection = (HttpsURLConnection) url.openConnection();

            String Line;
            StringBuilder ResponseContent = new StringBuilder();

            urlConnection.setConnectTimeout(5000);
            urlConnection.setReadTimeout(5000);
            urlConnection.setRequestMethod("POST");
            urlConnection.setRequestProperty("Content-Type",
                    "application/json");
            urlConnection.setRequestProperty("Accept", "application/json");
            JSONObject JObject = new JSONObject();
            JObject.put("pointOfInterestId", POIid);
            JObject.put("UserKey", UserKey);
            JObject.put("Region", region);
            try(OutputStream os = urlConnection.getOutputStream()){
                byte[] input = JObject.toString().getBytes(StandardCharsets.UTF_8);
                os.write(input,0,input.length);
            }

            try(BufferedReader br = new BufferedReader(
                    new InputStreamReader(urlConnection.getInputStream(), "utf-8")
            )){

                while ((Line = br.readLine()) != null){

                    ResponseContent.append(Line);

                }
                br.close();
            }
            int Code = urlConnection.getResponseCode();
            if(Code != 200 && Code != 201){
                System.out.println(Code + "Web Error");
                throw new Exception("Bad Response");
            }

        }
        catch (Exception e){
            e.printStackTrace();
            error = true;
        }
        finally {
            urlConnection.disconnect();
        }


        //
        if(error == true)
            this.c.FailedCallback();
        else
            this.c.callback();
    }
}
