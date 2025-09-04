# VelogPoster

Velog.io 포스트를 자동으로 제작하여 임시 저장하는 C# 콘솔 애플리케이션입니다.
글 발행은 Playwright 웹페이지에서 실행할 수 없으므로, 직접 발행해야 합니다.
자세한 설명은 [티스토리 블로그 포스트](https://develop-jen.tistory.com/entry/%EA%B0%80%EC%9D%B4%EB%93%9C-n8n%EC%9C%BC%EB%A1%9C-%EB%B2%A8%EB%A1%9C%EA%B7%B8-%EB%B8%94%EB%A1%9C%EA%B9%85-%EC%9E%90%EB%8F%99%ED%99%94)에서 확인하실 수 있습니다.
이 프로젝트는 n8n 노드로 활용하기 위해 제작되었습니다.

<img src="/docs/2_n8n.png">

## 기능

- Playwright를 사용하여 웹 브라우저를 자동화합니다.
- GitHub 계정을 통해 Velog에 로그인합니다.
- HTTP POST 요청으로 제목과 본문을 받아 Velog 글쓰기 페이지에 입력합니다.
- 작성된 글을 임시 저장합니다.

## 기술 스택

- C#
- .NET 9
- Playwright

## 사용법

1.  **프로젝트 실행**

    아래 명령어를 사용하여 프로젝트를 실행합니다. Velog 로그인에 사용하는 GitHub 이메일과 비밀번호를 인자로 전달해야 합니다.

    ```bash
    dotnet run VelogPoster <YOUR_GITHUB_EMAIL> <YOUR_GITHUB_PASSWORD>
    ```

    프로그램이 실행되면 `http://localhost:51205`에서 HTTP POST 요청을 기다립니다.

2.  **포스트 데이터 전송**

    별도의 터미널에서 `curl`과 같은 도구를 사용하여 작성할 글의 제목과 본문을 JSON 형식으로 전송합니다.

    ```bash
    curl -X POST -H "Content-Type: application/json" -d "{"title":"테스트 제목", "content":"테스트 본문입니다."}" http://localhost:51205/
    ```

    요청이 성공적으로 처리되면, 프로그램은 브라우저를 열어 자동으로 Velog에 로그인하고 글을 작성한 후 임시 저장합니다.

## n8n 연동 사용

1. n8n 에 블로그 글 주제를 입력합니다. 
<img src="/docs/1_input.png">

2. n8n이 작동하며, 자동으로 글을 작성합니다.
<img src="/docs/2_n8n.png">
<img src="/docs/3_velog.png">

3. 글이 임시저장됩니다. 이후 유저가 직접 벨로그에 접속하여 글을 발행합니다.
<img src="/docs/4_deploy.png">