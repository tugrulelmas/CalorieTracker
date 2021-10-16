import Link from 'next/link';
import { useRouter } from 'next/router';
import { userService } from '../services/user-service';

export default function SideBar() {
    const { asPath } = useRouter();
    const routes = [
        { icon: 'home', path: '/', text: 'Food Entry', needAdmin: false },
        { icon: 'list_alt', path: '/calories', text: 'Calories', needAdmin: false },
        { icon: 'summarize', path: '/admin/report', text: 'Reports', needAdmin: true },
        { icon: 'people', path: '/admin/entries', text: 'User Entries', needAdmin: true }
    ];

    return (
        <>
            <aside className="z-20 hidden w-64 overflow-y-auto bg-white md:block flex-shrink-0">
                <div className="py-4 text-gray-500">
                    <Link href="/">
                        <a className="ml-6 text-lg font-bold text-gray-800">Calorie Tracker </a>
                    </Link>

                    <ul className="mt-6">
                        {routes.map(route =>
                            ((route.needAdmin && userService.isAdmin()) || !route.needAdmin) && <li className="relative px-6 py-3" key={route.text}>
                                {asPath === route.path && <span className="absolute inset-y-0 left-0 w-1 bg-purple-600 rounded-tr-lg rounded-br-lg" aria-hidden="true"></span>}
                                <Link href={route.path}>
                                    <a
                                        className={"inline-flex items-center w-full text-sm font-semibold transition-colors duration-150 hover:text-gray-800" + (asPath === route.path ? " text-gray-800" : "")}>
                                        <span className="material-icons material-icons-outlined">{route.icon}</span>
                                        <span className="ml-4">{route.text}</span>
                                    </a>
                                </Link>
                            </li>
                        )}
                    </ul>
                </div>
            </aside>
        </>
    );
}