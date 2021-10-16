import { useState, useEffect } from 'react';
import { userService } from '../services/user-service';

export default function Header() {
    const [user, setUser] = useState(null);

    useEffect(() => {
        const subscription = userService.user.subscribe(x => setUser(x));
        return () => subscription.unsubscribe();
    }, []);

    const logout = () => {
        userService.logout();
    }

    return (
        <header className="z-10 py-4 bg-white shadow-md dark:bg-gray-800">
            <div className="container flex items-center justify-between h-full px-6 mx-auto text-purple-600 dark:text-purple-300">
                <div className="flex justify-center flex-1 lg:mr-32">
                    <div className="relative w-full max-w-xl mr-6 focus-within:text-purple-500">
                    </div>
                </div>
                <ul className="flex items-center flex-shrink-0 space-x-6">
                    <li className="relative">
                        {user && <button
                            onClick={logout}
                            className="align-middle rounded-full focus:shadow-outline-purple focus:outline-none"
                            aria-label="Account"
                            aria-haspopup="true">
                            logout {user.name}
                        </button>}
                    </li>
                </ul>
            </div>
        </header>
    );
}