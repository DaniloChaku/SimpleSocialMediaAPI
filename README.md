# SimpleSocialMediaAPI

## Setting Up 

### Prerequisites

- Git installed on your machine
- Docker and Docker Compose installed

### Step-by-Step Guide

### 1. Clone the Repository

```bash
git clone https://github.com/DaniloChaku/SimpleSocialMediaAPI.git
cd SimpleSocialMediaAPI
```

### 2. Set Up Environment Variables

```bash
cp .env.example .env
```

Open the `.env` file in your preferred text editor and fill in the required values.

### 3. Build and Run with Docker Compose

Once your environment variables are configured, you can start the application:

```bash
docker-compose up -d
```

The `-d` flag runs the containers in detached mode (in the background).

### 4. Check the Running Containers

Verify that your containers are running properly:

```bash
docker-compose ps
```

### 5. Access Your Application

Once the containers are up and running, you can access the application at the configured port (the default is http://localhost:5000). You can interact with the application on http://localhost:5000/swagger

## Quality Analysis
The coverage is less than 80% because I am required to write unit tests, while it makes more sense to write integration tests for the repository classes. Link: https://sonarcloud.io/project/overview?id=danilochaku_simplesocialmediaapi.

## AI Task Completion Feedback
- Was it easy to complete the task using **AI**?
  
  Yes, while I still had to do some things manually, AI was of great help.

- How long did task take you to complete? (*Please be honest, we need it to gather anonymized statistics*)
  
  4-5 hours. I spent a lot of time trying to configure GitHub workflow. Also, I think I slightly overdid the task, or it is long itself compared to the other 2 tasks. If I had chosen another one, I would have spent much less time.

- Was the code ready to run after generation? What did you have to change to make it usable?
  
  The code was ready overall, but I had to ask for some improvements in quality and additional features. However, it failed to properly use FakeLogger provided by Microsoft as a package. Also, it couldn't set up test coverage collection for SonarCloud.

- Which challenges did you face during completion of the task?
  
  The most challenging part was setting up test coverage collection for SonarCloud, which AI failed to do.

- Which specific prompts you learned as a good practice to complete the task?
  
  I didn't really learn any specific prompts. The pattern I used was task + [technologies] + [examples]
