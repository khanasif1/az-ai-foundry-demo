import os
import streamlit as st
import requests
from azure.storage.blob import BlobServiceClient
from dotenv import load_dotenv
import streamlit.components.v1 as components

# Load environment variables
load_dotenv()

# Azure Storage Configuration
sas_token = os.getenv("AZURE_STORAGE_SAS")
storage_account_name = os.getenv("AZURE_STORAGE_ACCOUNT_NAME")
container_name = os.getenv("AZURE_CONTAINER_NAME")

print("Variables: ", sas_token, storage_account_name, container_name)
# Construct the Blob Service Client using the SAS token
blob_service_client = BlobServiceClient(
    f"https://{storage_account_name}.blob.core.windows.net?{sas_token}"
)

# Define API URL (Replace with your actual API endpoint)
API_URL = "http://localhost:5119/api/DocumentAi"

def upload_file(file):
    """Uploads a file to Azure Blob Storage and returns the blob URL."""
    print("************************************")
    print("Uploading file: ", file.name)
    try:
        blob_client = blob_service_client.get_blob_client(container=container_name, blob=file.name)
        blob_client.upload_blob(file, overwrite=True)
        blob_url = f"https://{storage_account_name}.blob.core.windows.net/{container_name}/{file.name}"
        print("Blob URL: ", blob_url)
        print("************************************")
        return blob_url
    except Exception as e:
        st.error(f"File upload failed: {str(e)}")
        return None

# Streamlit UI
st.title("Azure Blob File Upload & API Call")

# File Upload Section
uploaded_file = st.file_uploader("Choose a file", type=["csv", "txt", "png", "jpg", "pdf"])

# Upload button
if uploaded_file is not None:
    st.write("Uploading file...")
    blob_url = upload_file(uploaded_file)
    
    if blob_url:
        st.success(f"File uploaded successfully! Blob URL: {blob_url}")
        st.text_input("Uploaded File URL", blob_url, disabled=True)

        # Text Input Section
        user_text = st.text_area("Enter your text")

        # Submit Button
        if st.button("Submit"):
            if not user_text:
                st.error("Please enter some text before submitting.")
            else:
                print("************************************")
                print("Call API")                
                # Call API with Blob URL and user input
                payload = {"blob_url": blob_url, "text": user_text}
                
                print ("API URL: ",f'{API_URL}?prompt={user_text}&filePath={blob_url}')
                response = requests.get(f'{API_URL}?prompt={user_text}&filePath={blob_url}')

                if response.status_code == 200:
                    # st.success("API call successful!")
                    print(response.content)
                    html_response = response.content
                    html_response = html_response.decode('utf-8') if isinstance(html_response, bytes) else html_response
                    html_response = html_response.replace('\\n', '\n').strip("'")
                    components.html(html_response, height=800, scrolling=True)
                    # st.html(response.content)
                    # st.json(response.json())  # Display API response
                    
                else:
                    st.error(f"API call failed: {response.status_code}")
                    st.json(response.json())
                print("************************************")

