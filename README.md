# ELK Notifier

ELK Notifier is a C# console application that automatically fetches alerts from an Elasticsearch index and sends them to a Webhook whenever new data is detected. It periodically checks the index for new data and performs the required actions automatically.

## Table of Contents
- [Overview](#overview)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [Dependencies](#dependencies)
- [Contributing](#contributing)
- [License](#license)

## Overview

This application periodically makes a REST call to an Elasticsearch index to fetch alerts. When new data is detected, the data is sent to a pre-configured Webhook URL.

## Installation

Follow these steps to set up and install the project:

1. **Clone the repository:**
   ```bash
   git clone https://github.com/your-username/elk-notifier.git
   cd elk-notifier
2. **Add the required NuGet packages:**
    ```bash
    dotnet add package NLog
    dotnet add package RestSharp
    dotnet add package Newtonsoft.Json
    dotnet add package DotNetEnv
3. **Build the project:**
   ```dotnet build```

## Configuration
The project uses a .env file to store configuration settings. Create a .env file in the root directory of your project with the following content:
    
    ELASTICSEARCH_URL=http://localhost:9200/your-index/_search
    WEBHOOK_URL=https://your-webhook-url.com

- **ELASTICSEARCH_URL:** The URL of your Elasticsearch index.
- **WEBHOOK_URL:** The URL of the Webhook to which new alerts will be sent.

## Usage
Start the application with the following command:
    ```dotnet run```

## Dependencies
This application uses the following NuGet packages:

- NLog: For logging and debugging.
- RestSharp: For making HTTP requests.
- Newtonsoft.Json: For parsing JSON data.
- DotNetEnv: For loading environment variables from a .env file.

## Contributing
Contributions to this project are welcome! Feel free to submit pull requests or create issues in the repository. Please ensure that your code adheres to the project's coding standards.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.
