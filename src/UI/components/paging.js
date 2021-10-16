export default function Paging({ page, pageSize, count, totalCount, onPageChange }) {
    const pageChange = (increment) => {
        const newPage = page + increment;
        if (newPage <= 0)
            return;

        if (newPage > Math.ceil(totalCount / pageSize))
            return;

        onPageChange(newPage);
    }

    return (
        <div className="grid px-4 py-3 text-xs font-semibold tracking-wide text-gray-500 uppercase border-t bg-gray-50 sm:grid-cols-9">
            <span className="flex items-center col-span-3">
                Showing  {((page - 1) * pageSize) + 1}-{((page - 1) * pageSize) + count} of {totalCount}
            </span>
            <span className="col-span-2"></span>
            <span className="flex col-span-4 mt-2 sm:mt-auto sm:justify-end">
                <nav aria-label="Table navigation">
                    <ul className="inline-flex items-center">
                        <li>
                            <button className="px-3 py-1 rounded-md rounded-l-lg focus:outline-none focus:shadow-outline-purple" aria-label="Previous"
                                onClick={() => pageChange(-1)}>
                                <svg aria-hidden="true" className="w-4 h-4 fill-current" viewBox="0 0 20 20">
                                    <path d="M12.707 5.293a1 1 0 010 1.414L9.414 10l3.293 3.293a1 1 0 01-1.414 1.414l-4-4a1 1 0 010-1.414l4-4a1 1 0 011.414 0z" clipRule="evenodd" fillRule="evenodd"></path>
                                </svg>
                            </button>
                        </li>
                        <li>
                            <button className="px-3 py-1 text-white transition-colors duration-150 bg-purple-600 border border-r-0 border-purple-600 rounded-md focus:outline-none focus:shadow-outline-purple">
                                {page}
                            </button>
                        </li>
                        <li>
                            <button className="px-3 py-1 rounded-md rounded-r-lg focus:outline-none focus:shadow-outline-purple" aria-label="Next"
                                onClick={() => pageChange(1)}>
                                <svg className="w-4 h-4 fill-current" aria-hidden="true" viewBox="0 0 20 20">
                                    <path d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z" clipRule="evenodd" fillRule="evenodd"></path>
                                </svg>
                            </button>
                        </li>
                    </ul>
                </nav>
            </span>
        </div>
    )
}