import { userService } from '../../services/user-service';
import Paging from '../../components/paging';

export default function CalorieList({ page, pageSize, totalCount, calories, onPageChange }) {

    if (!calories || calories.length === 0)
        return <></>;

    return (
        <div className="w-full mb-8 overflow-hidden rounded-lg shadow-xs">
            <div className="w-full overflow-x-auto">
                <table className="w-full whitespace-no-wrap">
                    <thead>
                        <tr className="text-xs font-semibold tracking-wide text-left text-gray-500 uppercase border-b bg-gray-50">
                            <th className="px-4 py-3">Date</th>
                            <th className="px-4 py-3 text-center">Calorie</th>
                        </tr>
                    </thead>
                    <tbody className="bg-white divide-y">
                        {calories.map(f =>
                            <tr className="text-gray-700" key={f.date}>
                                <td className="px-4 py-3 text-sm">
                                    {new Date(f.date).toLocaleDateString()}
                                </td>
                                <td className="px-4 py-3 text-sm text-center">
                                    {f.calorie < userService.userValue.maximumCalorie && f.calorie}
                                    {f.calorie >= userService.userValue.maximumCalorie && <span className="px-2 py-1 font-semibold leading-tight text-red-700 bg-red-100 rounded-full">
                                        {f.calorie}
                                    </span>}
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
            <Paging page={page} pageSize={pageSize} count={calories.length} totalCount={totalCount} onPageChange={onPageChange} />
        </div>
    )
}