# import streamlit as st
# from azure.storage.blob import BlobServiceClient
# import requests
# import os
# from io import BytesIO
# from dotenv import load_dotenv


# # Load environment variables
# load_dotenv()

# # Azure Storage Configuration
# AZURE_STORAGE_CONNECTION_STRING = os.getenv("AZURE_STORAGE_CONNECTION_STRING")
# AZURE_CONTAINER_NAME = os.getenv("AZURE_CONTAINER_NAME")

# # Mock API endpoint
# MOCK_API_URL = "https://jsonplaceholder.typicode.com/posts"

# def upload_file_to_azure(file):
#     """Uploads a file to Azure Blob Storage and returns the blob URL."""
#     try:
#         blob_service_client = BlobServiceClient.from_connection_string(AZURE_STORAGE_CONNECTION_STRING)
#         blob_client = blob_service_client.get_blob_client(container=AZURE_CONTAINER_NAME, blob=file.name)
        
#         # Upload file to Azure
#         blob_client.upload_blob(file, overwrite=True)
        
#         # Get the file URL
#         blob_url = f"https://{blob_service_client.account_name}.blob.core.windows.net/{AZURE_CONTAINER_NAME}/{file.name}"
#         return blob_url
#     except Exception as e:
#         st.error(f"Error uploading file: {e}")
#         return None

# def send_to_api(file_url, user_text):
#     """Sends file path and text to the API and returns response."""
#     payload = {
#         "file_url": file_url,
#         "text": user_text
#     }
#     response = requests.get(MOCK_API_URL, json=payload)
#     if response.status_code == 201:
#         return response.json().get("body", "<h1>Mock API Response</h1><p>This is a sample HTML response.</p>")
#     else:
#         st.error("Failed to get response from the API.")
#         return None

# # Streamlit UI
# st.title("Upload PDF to Azure & Process via API")

# # File uploader
# uploaded_file = st.file_uploader("Upload a PDF file", type=["pdf"])

# # Text input
# user_text = st.text_area("Enter additional text")

# if uploaded_file and user_text:
#     if st.button("Upload & Process"):
#         with st.spinner("Uploading file..."):
#             file_url = upload_file_to_azure(uploaded_file)
        
#         if file_url:
#             st.success(f"File uploaded successfully! URL: {file_url}")
#             with st.spinner("Sending data to API..."):
#                 api_response = send_to_api(file_url, user_text)

#             if api_response:
#                 st.success("Received response from API:")
#                 st.markdown(api_response, unsafe_allow_html=True)

import os
import streamlit as st
from azure.identity import ClientSecretCredential
from azure.storage.blob import BlobServiceClient
from dotenv import load_dotenv

# Load environment variables from .env file (if used)
load_dotenv()

# Get environment variables
storage_account_name = os.getenv("AZURE_STORAGE_ACCOUNT")
client_id = os.getenv("AZURE_CLIENT_ID")
tenant_id = os.getenv("AZURE_TENANT_ID")
client_secret = os.getenv("AZURE_CLIENT_SECRET")

# Azure AD authentication using service principal
credential = ClientSecretCredential(client_id=client_id, tenant_id=tenant_id, client_secret=client_secret)

# Blob service client
blob_service_client = BlobServiceClient(
    account_url=f"https://{storage_account_name}.blob.core.windows.net",
    credential=credential
)

# Streamlit UI
st.title("Upload File to Azure Blob Storage")

uploaded_file = st.file_uploader("Choose a file to upload", type=["csv", "txt", "png", "jpg"])

if uploaded_file is not None:
    container_name = "your-container-name"
    blob_client = blob_service_client.get_blob_client(container=container_name, blob=uploaded_file.name)

    blob_client.upload_blob(uploaded_file, overwrite=True)
    st.success(f"File {uploaded_file.name} uploaded successfully to {container_name}!")
