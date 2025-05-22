# iot-webapp-2025
IoT 개발자 과정 ***ASP.NET Core*** 학습 리포지토리

## 1일차

### Web
- 인터넷 상에 사용되는 서비스 중 하나
- 웹을 표현하는 기술 : HTML(Hyper Text Markup Language). XML(eXtemsable Markup Language)의 경량화 버전
- 2014년 이후 HTML5로 적용되고 있음

#### 웹기술
- HTML5(웹 페이지 구조) + CSS3(디자인) + JavaScript 6(인터렉티브)
- 웹 `서버`기술(백엔드) : ASP.NET Core(C#|VB), SpringBoot(Java), Flask|dJango(Python), CGI(PHP,C), Ruby, ...
- 웹 서비스 : 프론트엔드 + 백엔드
- 웹 브라우저 상에서 동작 : 현재는 웹 브라우저 상에서만 동작하는 경계가 사라졌음

#### HTML 5
- 웹 페이지를 구성하는 언어(근간, 기본)
- HTML 상에서도 디자인을 할 수 있으나, 현재는 CSS로 분리

#### CSS 3
- Cascading Style Sheet : 객체지향에 사용되는 부모자식관계로 디자인 하는 기술
- 아주 쉬운 문법으로 구성됨

#### JavaScript
- 표준명 ECMAScript 2024
- Java와 전혀 관계없음. Java의 문법을 차용해서 사용한 웹 스크립트 언어
- 엄청난 발전을 이뤄 여러가지 기술로 분리
    - React.js, View.js 등의 프론트엔드 기술 언어로 분파
    - Node.js와 같은 웹 서버기술에도 적용
    - Vs Code 같은 개발도구를 만드는데도 사용
    - 3D 게임, 모바일 개발 등 다양한 분야에 사용

#### 웹 서버기술
- `ASP.NET Core` : C#/VB언어도 웹 서버를 개발
- SpringBoot, Flask 등 다른언어로 웹 서버를 개발해도 무방

#### 왜 웹을 공부해야하나?
- 스마트팩토리 솔루션을 대부분 웹으로 개발(사용범위 제약을 없애기 위해서)
    - 웹사이트, 일부분 모바일 앱
- 어디서나 사용 가능한 회사 스마트팩토리 솔루션을 위해서 웹 서비스 + 모바일 + 웹서버 운영
- 스마트홈(IOT), ERP, 병원예약, 호텔예약, 인터넷뱅킹, 온라인서점 ...
- 모든 IT/ICT 개발에 웹 기술은 포함되어 있음

### HTTP
- HyterText Transfer Protocol
- 웹을 요청 / 응답하는 프로토콜
- HTTPs : HTTP with secute. 보안을 강화한 HTTP 프로토콜

### 웹 표준기술 학습

#### VS Code 확장설치
- Live Server

#### HTML 구조
- html 태그 내에 head, body로 구성(무조건!)
- README.md에도 HTML 태그를 그대로 사용가능(heading은 적용안됨)
- VS Code에서 html:5 자동생성
- CSS가 소스라인을 많이 사용. css는 외부스타일로 분리 사용
- JS도 소스라인이 매우 김. JS도 외부스크립트로 분리 사용
- 웹 브라우저의 개발자모드(F12)로 디버깅을 하는 것이 일반적

## 2일차

## 3일차