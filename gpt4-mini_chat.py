# import os  
# import base64
# from openai import AzureOpenAI  
# from azure.identity import DefaultAzureCredential, get_bearer_token_provider  
        
# endpoint = os.getenv("ENDPOINT_URL", "https://ai-hub-demo-basemodel.openai.azure.com/")  
# deployment = os.getenv("DEPLOYMENT_NAME", "gpt-4o-mini")  
      
# # Initialize Azure OpenAI client with Entra ID authentication
# token_provider = get_bearer_token_provider(  
#     DefaultAzureCredential(),  
#     "https://cognitiveservices.azure.com/.default"  
# )  
  
# client = AzureOpenAI(  
#     azure_endpoint=endpoint,  
#     azure_ad_token_provider=token_provider,  
#     api_version="2024-05-01-preview",  
# )  
  

# IMAGE_PATH = "YOUR_IMAGE_PATH"
# # encoded_image = base64.b64encode(open(IMAGE_PATH, 'rb').read()).decode('ascii')
# chat_prompt = [
#     {
#         "role": "system",
#         "content": [
#             {
#                 "type": "text",
#                 "text": "You are an AI assistant that helps people find information. Focus on chat in context of shoes and socks."
#             }
#         ]
#     },
#     {
#         "role": "user",
#         "content": [
#             {
#                 "type": "text",
#                 "text": "can you recommends something for my feet i live in Australia"
#             }
#         ]
#     },
#     {
#         "role": "assistant",
#         "content": [
#             {
#                 "type": "text",
#                 "text": "Sure! When it comes to shoes and socks in Australia, it often depends on your lifestyle and preferences. Here are a few recommendations:\n\n### Shoes:\n1. **Sneakers**: Brands like **Nike**, **Adidas**, and **New Balance** offer great options for casual wear and workouts. Look for breathable materials, especially in warmer climates.\n2. **Sandals**: If you’re looking for something more open, brands like **Teva** and **Birkenstock** provide comfortable and supportive sandals perfect for summer.\n3. **Boots**: For cooler months, consider **Blundstone** or **RM Williams** for durable and stylish options that can handle various terrains.\n4. **Dress Shoes**: If you need something for formal occasions, **Bared Footwear** and **Wittner** have fashionable options that prioritize comfort.\n\n### Socks:\n1. **Cotton Socks**: Great for everyday wear, brands like **Bonds** and **Hanes** offer soft cotton options that are breathable.\n2. **Wool Socks**: For cooler weather, merino wool socks from brands like **Icebreaker** are warm and moisture-wicking.\n3. **Athletic Socks**: If you’re active, look for moisture-wicking and cushioned socks from brands like **Asics** or **Nike**.\n\n### Tips:\n- **Fit**: Make sure to get the right fit for both shoes and socks to avoid blisters and discomfort.\n- **Climate Consideration**: Since Australia can get quite warm, look for shoes and socks made from breathable materials.\n\nLet me know if you need more specific recommendations or have a particular activity in mind!"
#             }
#         ]
#     }
# ] 
    
# # Include speech result if speech is enabled  
# messages = chat_prompt 

# completion = client.chat.completions.create(  
#     model=deployment,  
#     messages=messages,
#     max_tokens=800,  
#     temperature=0.7,  
#     top_p=0.95,  
#     frequency_penalty=0,  
#     presence_penalty=0,  
#     stop=None,  
#     stream=False  
# )  
  
# print(completion.to_json())  


import os  
import base64
from openai import AzureOpenAI  

endpoint = os.getenv("ENDPOINT_URL", "https://ai-hub-demo-basemodel.openai.azure.com/")  
deployment = os.getenv("DEPLOYMENT_NAME", "gpt-4o-mini")  
subscription_key = os.getenv("AZURE_OPENAI_API_KEY", "")  

# Initialize Azure OpenAI client with key-based authentication    
client = AzureOpenAI(  
    azure_endpoint=endpoint,  
    api_key=subscription_key,  
    api_version="2024-05-01-preview",  
)
    
    
IMAGE_PATH = "YOUR_IMAGE_PATH"
# encoded_image = base64.b64encode(open(IMAGE_PATH, 'rb').read()).decode('ascii')

#Prepare the chat prompt 
chat_prompt = [
    {
        "role": "system",
        "content": [
            {
                "type": "text",
                "text": "You are an AI assistant that helps people find information. Focus on chat in context of shoes and socks."
            }
        ]
    },
    {
        "role": "user",
        "content": [
            {
                "type": "text",
                "text": "tell me top five shoe brand"
            }
        ]
    },
    {
        "role": "assistant",
        "content": [
            {
                "type": "text",
                "text": "Sure! When it comes to shoes and socks in Australia, it often depends on your lifestyle and preferences. Here are a few recommendations:\n\n### Shoes:\n1. **Sneakers**: Brands like **Nike**, **Adidas**, and **New Balance** offer great options for casual wear and workouts. Look for breathable materials, especially in warmer climates.\n2. **Sandals**: If you’re looking for something more open, brands like **Teva** and **Birkenstock** provide comfortable and supportive sandals perfect for summer.\n3. **Boots**: For cooler months, consider **Blundstone** or **RM Williams** for durable and stylish options that can handle various terrains.\n4. **Dress Shoes**: If you need something for formal occasions, **Bared Footwear** and **Wittner** have fashionable options that prioritize comfort.\n\n### Socks:\n1. **Cotton Socks**: Great for everyday wear, brands like **Bonds** and **Hanes** offer soft cotton options that are breathable.\n2. **Wool Socks**: For cooler weather, merino wool socks from brands like **Icebreaker** are warm and moisture-wicking.\n3. **Athletic Socks**: If you’re active, look for moisture-wicking and cushioned socks from brands like **Asics** or **Nike**.\n\n### Tips:\n- **Fit**: Make sure to get the right fit for both shoes and socks to avoid blisters and discomfort.\n- **Climate Consideration**: Since Australia can get quite warm, look for shoes and socks made from breathable materials.\n\nLet me know if you need more specific recommendations or have a particular activity in mind!"
            }
        ]
    }
] 
    
# Include speech result if speech is enabled  
messages = chat_prompt  
    
# Generate the completion  
completion = client.chat.completions.create(  
    model=deployment,  
    messages=messages,  
    max_tokens=800,  
    temperature=0.7,  
    top_p=0.95,  
    frequency_penalty=0,  
    presence_penalty=0,  
    stop=None,  
    stream=False
)

print(completion.to_json())  
    