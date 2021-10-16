import { useState, useEffect } from 'react';
import { useRouter } from 'next/router';
import Head from 'next/head';
import Sidebar from '../components/sidebar';
import Header from '../components/header';
import { userService } from '../services/user-service';
import '../styles/globals.css'


function MyApp({ Component, pageProps }) {
  const [authorized, setAuthorized] = useState(false);
  const [user, setUser] = useState(null);
  const router = useRouter();

  useEffect(() => {
    authCheck(router.asPath);

    const hideContent = () => setAuthorized(false);
    router.events.on('routeChangeStart', hideContent);

    router.events.on('routeChangeComplete', authCheck)

    return () => {
      router.events.off('routeChangeStart', hideContent);
      router.events.off('routeChangeComplete', authCheck);
    }
  }, []);

  const authCheck = (url) => {
    setUser(userService.userValue);

    if (url === '/account/login') {
      setAuthorized(true);
      return;
    }

    if (!userService.userValue) {
      setAuthorized(false);
      return;
    }

    if (url.startsWith('/admin') && !userService.isAdmin()) {
      setAuthorized(false);
      return;
    }

    setAuthorized(true);
  }

  return (
    <>
      <Head>
        <title>Calorie Tracker</title>
        <meta name="description" content="Calorie tracker app" />
        <link rel="icon" href="/favicon.ico" />
        <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap" rel="stylesheet" />
        <link href="https://fonts.googleapis.com/css?family=Roboto:300,400,500" rel="stylesheet" />
        <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet"></link>
      </Head>
      <div className="flex h-screen bg-gray-50">
        {user && <Sidebar />}
        <div className="flex flex-col flex-1">
          {user && <Header />}
          <main className="h-full pb-16 overflow-y-auto">
            {authorized && <Component {...pageProps} />}
          </main>
        </div>
      </div>
    </>)
}

export default MyApp
